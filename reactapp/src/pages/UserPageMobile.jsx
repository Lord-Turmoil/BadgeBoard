import { useEffect, useState } from 'react';

import isEqual from 'lodash.isequal';
import { useNavigate, useParams } from 'react-router-dom';

import moment from 'moment/moment';
import CakeRoundedIcon from '@mui/icons-material/CakeRounded';
import { faSpinner } from '@fortawesome/free-solid-svg-icons';
import MaleRoundedIcon from '@mui/icons-material/MaleRounded';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { AdapterMoment } from '@mui/x-date-pickers/AdapterMoment'
import FemaleRoundedIcon from '@mui/icons-material/FemaleRounded';
import { LocalizationProvider, MobileDatePicker } from '@mui/x-date-pickers';
import QuestionMarkRoundedIcon from '@mui/icons-material/QuestionMarkRounded';
import DriveFileRenameOutlineRoundedIcon from '@mui/icons-material/DriveFileRenameOutlineRounded';
import { Avatar, Button, Divider, FormControl, FormControlLabel, FormLabel, Grid, InputAdornment, Radio, RadioGroup, TextField } from '@mui/material';

import api from '../components/api';
import stall from '../components/stall';
import User from '../components/user/User';
import notifier from '../components/notifier';
import AvatarUrl from '../components/user/avatarUrl';
import InflateBox from '../components/layout/inflate';
import ExpandFab from '../components/utility/ExpandFab';
import SubtleInput from '../components/form/SubtleInput';
import { useLocalUser, useUser } from '../components/user/useUser';

import '../assets/css/pages/user/user.mobile.css'

/*
Parent could not get states of child component, but can use callback
to update their copy in parent.
*/
export default function UserPageMobile() {
    const navigate = useNavigate();
    const { uid } = useParams('uid');

    // current logged in user
    const {
        data: visitor,
        loading: visitorLoading,
        error: visitorError
    } = useLocalUser();

    useEffect(() => {
        console.log("🚀 > useEffect > visitor:", visitor);
    }, [visitor]);

    // current visiting user
    const {
        data: user,
        loading: userLoading,
        error: userError
    } = useUser(uid ? uid : (visitor ? visitor.account.id : null), () => {
        // waiting for visitor to complete loading
        if (visitorLoading) {
            navigate('/404');
        }
    });

    useEffect(() => {
        console.log("🚀 > useEffect > user:", user);
    }, [user]);

    useEffect(() => {
        if (userError) {
            notifier.error(userError.message);
            setTimeout(() => { navigate(-1) }, 1000);
        }
    }, [userError]);

    const isOwner = () => {
        if ((visitor == null) || (user == null)) {
            return false;
        }
        return visitor.account.id == user.account.id;
    }

    // expand toggle
    const [expandOn, setExpandOn] = useState(false);
    const toggleExpand = () => {
        setExpandOn(!expandOn);
    }

    // edit
    const getSexText = (no) => {
        switch (no) {
            case 1: return "Male";
            case 2: return "Female";
            default: return "Unknown";
        }
    }

    const getSexNo = (text) => {
        if (text == null) {
            return 0;
        }
        switch (text.toLowerCase()) {
            case "male": return 1;
            case "female": return 2;
            default: return 0;
        }
    }

    const [shadow, setShadow] = useState({});
    const [sex, setSex] = useState(getSexText(shadow.sex));

    useEffect(() => {
        setShadow(User.flat(user));
        console.log("🚀 > useEffect > getShadow(user):", User.flat(user));
    }, [user]);

    const [enableEdit, setEnableEdit] = useState(false);

    const turnOnEdit = () => {
        var flat = User.flat(user);
        setShadow(flat);
        setSex(getSexText(flat.sex));
        setEnableEdit(true);
    }

    const turnOffEdit = () => {
        setShadow(User.flat(user));
        setEnableEdit(false);
    }

    const [mottoError, setMottoError] = useState({ err: false, hint: "" });
    const [usernameError, setUsernameError] = useState({ err: false, hint: "" });

    useEffect(() => {
        if (shadow && shadow.motto) {
            checkMotto(shadow.motto);
        }
    }, [shadow]);

    useEffect(() => {
        if (shadow && shadow.username) {
            checkUsername(shadow.username);
        }
    }, [shadow]);

    const onMottoChange = (event) => {
        event.preventDefault();
        setShadow({ ...shadow, motto: event.target.value.trim().replace(/[\r\n]/g, '') });
    }

    const checkMotto = (str) => {
        if (str.length > 66) {
            setMottoError({ err: true, hint: `A little shorter? (${str.length}/66)` });
            return false;
        } else {
            setMottoError({ ...mottoError, err: false });
            return true;
        }
    }

    const getMotto = (str) => {
        if (str == null || str.length == 0) {
            return <span style={{ color: "rgba(0, 0, 0, 0.5)" }}>Nothing to say...</span>
        } else {
            return str;
        }
    }
    const onUsernameChange = (event) => {
        event.preventDefault();
        setShadow({ ...shadow, username: event.target.value.trim().replace(/[\r\n]/g, '') });
    }

    const checkUsername = (str) => {
        if (str.length > 16) {
            setUsernameError({ err: true, hint: `Too long for a name (${str.length}/16)` });
            return false;
        } else if (str.length < 3) {
            setUsernameError({ err: true, hint: `Too short for a name (${str.length}/3)` });
        } else {
            setUsernameError({ ...usernameError, err: false });
            return true;
        }
    }

    useEffect(() => {
        setShadow({ ...shadow, sex: getSexNo(sex) });
    }, [sex]);

    // Submitting

    const isReady = () => {
        return !(mottoError.err || usernameError.err);
    }

    const [onSubmitting, setOnSubmitting] = useState(false);
    const submitEdit = async (event) => {
        event.preventDefault();

        if (!isReady()) {
            return;
        }

        setOnSubmitting(true);
        var status = await stall(submitAll(), 500);
        if (status) {
            notifier.success("Profile updated!");
            turnOffEdit();
        }
        setOnSubmitting(false);
    }

    const submitAll = async () => {
        var ret = true;
        if (!await submitUserInfo()) {
            ret = false;
        }
        if (!await submitUsername()) {
            ret = false;
        }
        return ret;
    }

    const submitUserInfo = async () => {
        if (isEqual(user.info, {
            motto: shadow.motto,
            birthday: shadow.birthday,
            sex: shadow.sex
        })) {
            return true;
        }

        var dto = await api.post("user/info", {
            motto: (shadow.motto == user.info.motto) ? null : shadow.motto,
            birthday: (shadow.birthday == user.info.motto) ? null : shadow.birthday,
            sex: (shadow.sex == user.info.sex) ? null : shadow.sex
        });
        console.log("🚀 > submitUserInfo > dto :", dto);
        if (dto.meta.status != 0) {
            notifier.error(dto.meta.message);
            return false;
        }
        setShadow({ ...shadow, ...dto.data });
        user.info = dto.data;
        visitor.info = dto.data;
        return true;
    }

    const submitUsername = async () => {
        if (shadow.username == user.username) {
            return true;
        }

        var dto = await api.post("user/username", { username: shadow.username });
        console.log("🚀 > submitUsername > dto:", dto);
        if (dto.meta.status != 0) {
            notifier.error(dto.meta.message);
            return false;
        }
        setShadow({ ...shadow, username: dto.data });
        user.username = dto.data;
        visitor.username = dto.data;
        return true;
    }

    // data format
    const formatBirthday = (day) => {
        if (day == null) {
            return null;
        } else if (day == "") {
            return moment();
        }
        return moment(day, "YYYY-MM-DD");
    }

    const getBirthday = (str) => {
        if (str == null || str.length == 0) {
            return <span style={{ color: "rgba(0, 0, 0, 0.5)" }}>????-??-??</span>
        } else {
            return str;
        }
    }
    
    return (
        <div className="user-mobile-main">
            <div className="nav-wrapper">
                <ExpandFab disabled={enableEdit} onClick={toggleExpand} />
                <div className="nav"></div>
            </div>
            <div className={`panel${expandOn ? ' active' : ''}`}>
                <div className="primary">
                    <div className="avatar">
                        <Avatar sx={{ width: 100, height: 100 }} src={AvatarUrl.get(shadow && shadow.avatarUrl)} />
                    </div>
                    <div className="info-wrapper">
                        <SubtleInput
                            cls='username'
                            sx={enableEdit ? { marginTop: "15px" } : null}
                            error={usernameError.err}
                            helperText={usernameError.hint}
                            placeholder='Username'
                            enabled={enableEdit}
                            defaultValue={shadow && shadow.username}
                            onChange={onUsernameChange}
                            variant='outlined'
                            label='Username'
                        />
                        <Divider sx={{ height: "1px", display: enableEdit ? "none" : "block" }} />
                        <div className="info">
                            {
                                (enableEdit ?
                                    <div className="birthday">
                                        <LocalizationProvider dateAdapter={AdapterMoment}>
                                            <MobileDatePicker
                                                disableFuture
                                                sx={enableEdit ? { marginTop: "15px" } : null}
                                                readOnly={!enableEdit}
                                                value={formatBirthday(shadow ? shadow.birthday : null)}
                                                onChange={(newValue) => { setShadow({ ...shadow, birthday: newValue.format("YYYY-MM-DD") }) }}
                                                label="Birthday" />
                                        </LocalizationProvider>
                                    </div>
                                    :
                                    <div className='birthday'>
                                        <CakeRoundedIcon sx={{ verticalAlign: 'bottom' }} />{getBirthday(shadow && shadow.birthday)}
                                    </div>)
                            }
                            <div className="motto" style={{ display: enableEdit ? "none" : "block" }}>
                                <DriveFileRenameOutlineRoundedIcon sx={{ verticalAlign: 'bottom' }} />{getMotto(shadow && shadow.motto)}
                            </div>
                        </div>
                        <div style={{ display: enableEdit ? "block" : "none" }}>
                            <FormControl fullWidth sx={enableEdit ? { marginTop: "5px" } : null}>
                                <FormLabel sx={{ fontSize: '0.8rem', paddingLeft: '14px' }}>Gender</FormLabel>
                                <RadioGroup row sx={{ justifyContent: 'center' }} value={sex} onChange={(event) => { setSex(event.target.value); }}>
                                    <FormControlLabel value="Unknown" control={<Radio color='error' />} label={<QuestionMarkRoundedIcon color='error' />} />
                                    <FormControlLabel value="Male" control={<Radio color='primary' />} label={<MaleRoundedIcon color='primary' />} />
                                    <FormControlLabel sx={{ marginRight: '0' }} value="Female" control={<Radio color='secondary' />} label={<FemaleRoundedIcon color='secondary' />} />
                                </RadioGroup>
                            </FormControl>
                        </div>
                    </div>
                </div>
                <Divider />
                {enableEdit ?
                    <div style={{ padding: "5px 10px" }}>
                        <TextField
                            fullWidth
                            sx={{ marginTop: "15px" }}
                            error={mottoError.err}
                            helperText={mottoError.err ? mottoError.hint : ""}
                            placeholder='Personalized signature'
                            multiline
                            defaultValue={shadow && shadow.motto}
                            onChange={onMottoChange}
                            variant='outlined'
                            label='Personalized Signature'
                            InputProps={{
                                endAdornment: (
                                    <InputAdornment position='end'>
                                        <DriveFileRenameOutlineRoundedIcon />
                                    </InputAdornment>
                                )
                            }} />
                    </div> : null
                }
                {isOwner() ?
                    <div className="edit">
                        {enableEdit ?
                            <Grid container spacing={2}>
                                <Grid item xs={6}>
                                    <Button fullWidth variant='contained' color='error' onClick={turnOffEdit}>Cancel</Button>
                                </Grid>
                                <Grid item xs={6}>
                                    <Button disabled={!isReady() || onSubmitting} fullWidth variant='contained' color='success' onClick={submitEdit}>
                                        {onSubmitting ? <span>&nbsp;<FontAwesomeIcon icon={faSpinner} spinPulse />&nbsp;</span> : "Done"}
                                    </Button>
                                </Grid>
                            </Grid>
                            : <Button fullWidth variant="contained" onClick={turnOnEdit}>Edit</Button>
                        }
                    </div>
                    : null
                }
            </div>
            <InflateBox sx={{ backgroundColor: 'lightBlue' }} overflow>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
            </InflateBox>
        </div>
    );
}
