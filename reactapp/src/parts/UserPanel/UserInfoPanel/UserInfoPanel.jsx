import { useEffect, useState } from 'react';

import isEqual from 'lodash.isequal';

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

import SubtleInput from '~/components/form/SubtleInput';

import api from '~/services/api';
import stall from '~/services/stall';
import User from '~/services/user/user';
import notifier from '~/services/notifier';
import AvatarUrl from '~/services/user/avatarUrl';

// Required in parent component
// import '~/assets/css/user.panel.define.css'
import './UserInfoPanel.css'

const MOTTO_MAX_LENGTH = 66;
const MOTTO_MIN_LENGTH = 0;
const USERNAME_MAX_LENGTH = 16;
const USERNAME_MIN_LENGTH = 3;

/*
Parent could not get states of child component, but can use callback
to update their copy in parent.
*/
export default function UserInfoPanel({
    user = {},
    visitor = {},
    onUserChange = null,
    onVisitorChange = null,
    disabled = true
}) {
    // user related
    const isOwner = () => {
        if ((visitor == null) || (user == null)) {
            return false;
        }
        return visitor.account.id == user.account.id;
    }

    const onUserChangeInner = (data) => {
        user && onUserChange && onUserChange(data);
        visitor && onVisitorChange && onVisitorChange(data);
    }

    // editing
    const [enableEdit, setEnableEdit] = useState(false);
    const [editKey, setEditKey] = useState(0);
    const turnOnEdit = () => {
        var flat = User.flat(user);
        setShadow(flat);
        setSex(User.getSexText(flat.sex));
        setEnableEdit(true);
        setEditKey(editKey + 1);
    }
    const turnOffEdit = () => {
        setShadow(User.flat(user));
        setEnableEdit(false);
        setEditKey(editKey + 1);
    }

    useEffect(() => {
        if (disabled) {
            turnOffEdit();
        }
    }, [disabled]);

    // edit properties
    const [shadow, setShadow] = useState({});
    const [sex, setSex] = useState(User.getSexText(shadow.sex));

    useEffect(() => {
        if (user) {
            var flat = User.flat(user);
            setShadow(flat);
            setSex(User.getSexText(flat.sex));
        }
    }, [user]);

    useEffect(() => {
        shadow && validateMotto(shadow.motto);
        shadow && validateUsername(shadow.username);
    }, [shadow]);

    useEffect(() => {
        setShadow({ ...shadow, sex: User.getSexNo(sex) });
    }, [sex]);

    const onMottoChange = (event) => {
        setShadow({ ...shadow, motto: event.target.value.trim().replace(/[\r\n]/g, '') });
    }

    const onUsernameChange = (event) => {
        setShadow({ ...shadow, username: event.target.value.trim().replace(/[\r\n]/g, '') });
    }

    // error handling
    const [mottoError, setMottoError] = useState({ err: false, hint: "" });
    const [usernameError, setUsernameError] = useState({ err: false, hint: "" });

    const validateMotto = (str) => {
        if (str == null) {
            setMottoError({ ...mottoError, err: false });
            return true;
        }

        if (str.length > MOTTO_MAX_LENGTH) {
            setMottoError({ err: true, hint: `A little shorter? (${str.length}/${MOTTO_MAX_LENGTH})` });
            return false;
        } else {
            setMottoError({ ...mottoError, err: false });
            return true;
        }
    }

    const validateUsername = (str) => {
        if (str == null) {
            setUsernameError({ ...usernameError, err: false });
            return true;
        }
        if (str.length > USERNAME_MAX_LENGTH) {
            setUsernameError({ err: true, hint: `Too long for a name (${str.length}/${USERNAME_MAX_LENGTH})` });
            return false;
        } else if (str.length < USERNAME_MIN_LENGTH) {
            setUsernameError({ err: true, hint: `Too short for a name (${str.length}/${USERNAME_MIN_LENGTH})` });
        } else {
            setUsernameError({ ...usernameError, err: false });
            return true;
        }
    }

    // submitting
    const isReady = () => {
        return !(mottoError.err || usernameError.err);
    }

    const [onSubmitting, setOnSubmitting] = useState(false);
    const onClickSubmit = async (event) => {
        if (!isReady()) {
            return;
        }

        setOnSubmitting(true);
        var status = await stall(submitAll());
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
        if (isEqual(user.info, { motto: shadow.motto, birthday: shadow.birthday, sex: shadow.sex })) {
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
        onUserChangeInner({ key: "info", value: dto.data });
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
        onUserChangeInner({ key: "username", value: dto.data });
        return true;
    }

    // data format and rendering
    const formatBirthday = (day) => {
        if (day == null) {
            return null;
        } else if (day == "") {
            return moment();
        }
        return moment(day, "YYYY-MM-DD");
    }

    const renderBirthday = (str) => {
        if (str == null || str.length == 0) {
            return <span style={{ color: "rgba(0, 0, 0, 0.5)" }}>????-??-??</span>
        } else {
            return str;
        }
    }

    const renderMotto = (str) => {
        if (str == null || str.length == 0) {
            return <span style={{ color: "rgba(0, 0, 0, 0.5)" }}>Nothing to say...</span>
        } else {
            return str;
        }
    }

    return (
        <div className={`UserInfoPanel${disabled ? '' : ' active'}`} key={editKey}>
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
                        {enableEdit ?
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
                                <CakeRoundedIcon sx={{ verticalAlign: 'bottom' }} />{renderBirthday(shadow && shadow.birthday)}
                            </div>
                        }
                        <div className="motto" style={{ display: enableEdit ? "none" : "block" }}>
                            <DriveFileRenameOutlineRoundedIcon sx={{ verticalAlign: 'bottom' }} />{renderMotto(shadow && shadow.motto)}
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
                                <Button disabled={!isReady() || onSubmitting} fullWidth variant='contained' color='success' onClick={onClickSubmit}>
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
    );
}
