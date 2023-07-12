import React, { useEffect, useRef, useState } from 'react';

import _debounce from 'debounce';
import { Helmet } from 'react-helmet';

import { Button } from '@mui/material';
import { faSpinner } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import AccountCircleRoundedIcon from '@mui/icons-material/AccountCircleRounded';

import api from '../components/api';
import stall from '../components/stall';
import BackNavBar from '../components/form/BackNavBar';
import PasswordField from '../components/form/PasswordField';
import PlainTextField from '../components/form/PlainTextField';

import '../assets/css/pages/form.css';


// const PASSWORD_REGEX = new RegExp(/^(?=.*\d)(?=(.*\W){1})(?=.*[a-zA-Z])(?!.*\s).{6,16}$/);
const PASSWORD_REGEX = new RegExp(/^[a-z0-9_-]{6,16}$/i);
const USERNAME_REGEX = new RegExp(/^[ a-z0-9_-]{3,20}$/i);

const CHECK_DELAY = 300;
const DUP_CHECK_DELAY = 3000;
var timer = null;    // none state variable should be declared outside...

var usernameText = "";
var passwordText = "";
var confirmText = "";

function setUsernameText(str) {
    usernameText = str;
}
function setPasswordText(str) {
    passwordText = str;
}
function setConfirmText(str) {
    confirmText = str;
}

export default function RegisterPage() {
    const usernameRef = useRef(null);
    const passwordRef = useRef(null);
    const confirmRef = useRef(null);

    const [username, setUsername] = useState({ value: "", error: false, hint: "" });
    const [password, setPassword] = useState({ value: "", error: false, hint: "" });
    const [confirm, setConfirm] = useState({ value: "", error: false, hint: "" });

    const onUsernameChange = (event) => {
        var newUsername = event.target.value.trim();
        var good = true;

        setUsernameText(newUsername);
        if ((newUsername.length > 0) && !USERNAME_REGEX.test(newUsername)) {
            setUsername({ ...username, value: newUsername, error: true, hint: "3 ~ 20 characters (a-zA-Z0-9_-)" });
            good = false;
        } else {
            setUsername({ ...username, value: newUsername, error: false });
        }

        // debounce
        if (timer) {
            clearTimeout(timer);
            timer = null;
        }
        if (good && newUsername.length > 0) {
            timer = setTimeout(async function () { await checkDuplication(newUsername); }, DUP_CHECK_DELAY);
        }
    }

    const onPasswordChange = (event) => {
        // it is async, so won't be updated right away
        var newPassword = event.target.value.trim();
        setPasswordText(newPassword);
        if ((newPassword.length > 0) && !PASSWORD_REGEX.test(newPassword)) {
            setPassword({ ...password, value: newPassword, error: true, hint: "6 ~ 16 characters (a-zA-Z0-9_-)" });
        } else {
            setPassword({ ...password, value: newPassword, error: false });
        }
        checkConfirm();
    }

    const onConfirmChange = (event) => {
        var newConfirm = event.target.value;
        setConfirmText(newConfirm);
        setConfirm({ ...confirm, value: newConfirm, error: false });
        checkConfirm();
    }

    // Bad async!!!
    const checkConfirm = () => {
        if (confirmText !== passwordText) {
            setConfirm({ ...confirm, value: confirmText, error: true, hint: "Passwords inconsistent" });
        } else {
            setConfirm({ ...confirm, value: confirmText, error: false, hint: "" });
        }
    }

    // action button
    const onClickReset = (event) => {
        setUsernameText("");
        setPasswordText("");
        setConfirmText("");
        setUsername({ ...username, value: "", error: false });
        setPassword({ ...password, value: "", error: false });
        setConfirm({ ...confirm, value: "", error: false });
        usernameRef.current.getElementsByTagName("input")[0].value = "";
        passwordRef.current.getElementsByTagName("input")[0].value = "";
        confirmRef.current.getElementsByTagName("input")[0].value = "";
    }

    const onClickSubmit = async (event) => {
        if (!isReady()) {
            return;
        }

        setLoading(true);

        if (timer) {
            clearTimeout(timer);
        }
        if (!await checkDuplication(usernameText)) {
            setLoading(false);
            return;
        }
        var dto = await stall(api.post("auth/register", {
            username: usernameText,
            password: passwordText
        }), 1000);

        console.log(dto);
        setLoading(false);
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
        var result = await isExist(name);
        if (result.status) {
            setUsername({ ...username, error: result.status, hint: result.message });
            return false;
        }
        // return operation succeeded or not, not existence
        return true;
    }

    const isExist = async (name) => {
        return await api.get('user/exists', {
            type: 'username',
            value: name
        }).then(dto => {
            return { status: dto.data, message: dto.data ? "Username already occupied" : "" };
        }).catch(dto => {
            return { status: true, message: "Server error, please stand by" };
        });
    }

    // loading effect
    const [loading, setLoading] = useState(false);

    return (
        <div>
            <Helmet>
                <title>Sign up</title>
            </Helmet>
            <div className="form-main">
                <BackNavBar></BackNavBar>
                <div className="wrapper">
                    <div className="dialog">
                        <div className="title">
                            <h1 className="font-hand">Sign Up</h1>
                        </div>
                        <div className="input-wrapper">
                            <div className="input-item">
                                <PlainTextField
                                    error={username.error}
                                    hint={username.hint}
                                    onChange={_debounce(onUsernameChange, CHECK_DELAY)}
                                    disabled={loading}
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
                                    disabled={loading}
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
                                    disabled={loading}
                                    sx={{ id: 'confirm', ref: confirmRef, label: 'Confirm Password' }} />
                            </div>
                        </div>

                        <div className="action-wrapper">
                            <div className='reset'>
                                <Button fullWidth variant='contained' onClick={loading ? null : onClickReset}>Reset</Button>
                            </div>
                            <div className='submit'>
                                <Button fullWidth variant='contained' color='success' disabled={!ready} onClick={loading ? null : onClickSubmit}>
                                    {loading ? <span>&nbsp;<FontAwesomeIcon icon={faSpinner} spinPulse />&nbsp;</span> : "Sign up"}
                                </Button>
                            </div>
                        </div>

                        <div className="login"></div>
                    </div>
                </div>
            </div>
        </div>
    );
}