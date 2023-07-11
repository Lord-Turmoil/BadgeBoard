import React, { useEffect, useRef, useState } from 'react';
import '../assets/css/pages/register.css'
import { Helmet } from 'react-helmet';
import PasswordField from '../components/form/PasswordField';
import PlainTextField from '../components/form/PlainTextField'
import BackNavBar from '../components/form/BackNavBar';
import AccountCircleRoundedIcon from '@mui/icons-material/AccountCircleRounded';
import _debounce from 'debounce';
import { Button } from '@mui/material';
import api from '../components/api';

// const PASSWORD_REGEX = new RegExp(/^(?=.*\d)(?=(.*\W){1})(?=.*[a-zA-Z])(?!.*\s).{6,16}$/);
const PASSWORD_REGEX = new RegExp(/^[a-z0-9_-]{6,16}$/i);
const USERNAME_REGEX = new RegExp(/^[ a-z0-9_-]{3,20}$/);

const CHECK_DELAY = 300;
const DUP_CHECK_DELAY = 3000;
var interval = null;    // none state variable should be declared outside...

export default function RegisterPage() {
    const usernameRef = useRef(null);
    const passwordRef = useRef(null);
    const confirmRef = useRef(null);

    const [password, setPassword] = useState({ value: "", error: false, hint: "" });
    const [confirm, setConfirm] = useState({ value: "", error: false, hint: "Passwords inconsistent" });
    const [username, setUsername] = useState({ value: "", error: false, hint: "" });


    const onUsernameChange = (event) => {
        var newUsername = event.target.value.trim();
        var good = true;
        if ((newUsername.length > 0) && !USERNAME_REGEX.test(newUsername)) {
            setUsername({ ...username, value: newUsername, error: true, hint: "3 ~ 20 characters (a-zA-Z0-9_-)" });
            good = false;
        } else {
            setUsername({ ...username, value: newUsername, error: false });
        }

        if (interval) {
            clearTimeout(interval);
        }
        if (good && newUsername.length > 0) {
            interval = setTimeout(function () { console.log("hello"); checkDuplication(newUsername); }, DUP_CHECK_DELAY);
        }
    }

    const onPasswordChange = (event) => {
        // it is async, so won't be updated right away
        var newPassword = event.target.value.trim();
        if ((newPassword.length > 0) && !PASSWORD_REGEX.test(newPassword)) {
            setPassword({ ...password, value: newPassword, error: true, hint: "6 ~ 16 characters (a-zA-Z0-9_-)" });
        } else {
            setPassword({ ...password, value: newPassword, error: false });
        }
    }

    const onConfirmChange = (event) => {
        setConfirm({ ...confirm, value: event.target.value });
    }

    useEffect(() => {
        checkConfirm();
    }, [password, confirm]);

    const checkConfirm = () => {
        if (confirm.value !== password.value) {
            if (!confirm.error) {
                setConfirm({ ...confirm, error: true });
            }
        } else if (confirm.error) {
            setConfirm({ ...confirm, error: false });
        }
    }

    // action button
    const onClickReset = (event) => {
        setUsername({ ...username, value: "", error: false });
        setPassword({ ...password, value: "", error: false });
        setConfirm({ ...confirm, value: "", error: false });
        usernameRef.current.getElementsByTagName("input")[0].value = "";
        passwordRef.current.getElementsByTagName("input")[0].value = "";
        confirmRef.current.getElementsByTagName("input")[0].value = "";
    }

    const onClickSubmit = (event) => {
        if (!isReady()) {
            return;
        }

        api.post("auth/register", {
            username: username.value,
            password: "a"
        }).then(res => {
            var dto = res.data;
            console.log(dto);
        }).catch(err => {
            console.error(err);
        });
    }

    // ready
    const [ready, setReady] = useState(false);

    useEffect(() => {
        if (isReady()) {
            setReady(true);
        } else {
            setReady(false);
        }
    }, [username, password, confirm]);

    const checkField = (field) => {
        return (!field.error) && (field.value.length > 0)
    };

    const isReady = () => {
        return (checkField(username) && checkField(password) && checkField(confirm));
    }

    // user name duplication check
    const checkDuplication = async (name) => {
        var status = await isExist(name);
        console.log(status);
        if (status) {
            setUsername({ ...username, error: true, hint: "Username already exists" });
        }
    }

    const isExist = async (name) => {
        return await api.get('user/exists', {
            params: {
                type: 'username',
                value: name
            }
        }).then(res => {
            var dto = res.data;
            console.log(dto);
            return dto.data;
        }).catch(err => {
            console.error(err);
            return false;
        });
    }

    return (
        <div>
            <Helmet>
                <title>Sign up</title>
            </Helmet>
            <div className="register-main">
                <BackNavBar></BackNavBar>
                <div className="wrapper">
                    <div className="dialog">
                        <div className="title">
                            <h1 className='font-hand'>Sign Up</h1>
                        </div>
                        <div className="input-wrapper">
                            <div className="input-item">
                                <PlainTextField
                                    error={username.error}
                                    hint={username.hint}
                                    onChange={_debounce(onUsernameChange, CHECK_DELAY)}
                                    sx={{
                                        id: 'username', ref: usernameRef, label: 'Username',
                                        icon: <AccountCircleRoundedIcon fontSize='large' />
                                    }} />
                            </div>
                            <div className="input-item">
                                <PasswordField
                                    error={password.error}
                                    hint={password.hint}
                                    onChange={_debounce(onPasswordChange, CHECK_DELAY)}
                                    sx={{
                                        id: 'password',
                                        ref: passwordRef
                                    }} />
                            </div>
                            <div className="input-item">
                                <PasswordField
                                    error={confirm.error}
                                    hint={confirm.hint}
                                    onChange={_debounce(onConfirmChange, CHECK_DELAY)}
                                    sx={{ id: 'confirm', ref: confirmRef, label: 'Confirm Password' }} />
                            </div>
                        </div>
                        <div className="action-wrapper">
                            <div className='reset'>
                                <Button fullWidth variant='contained' onClick={onClickReset}>Reset</Button>
                            </div>
                            <div className='submit'>
                                <Button fullWidth variant='contained' color='success' disabled={!ready} onClick={onClickSubmit}>Submit</Button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}