import React, { useEffect, useRef, useState } from 'react';

import _debounce from 'debounce';
import { Helmet } from 'react-helmet';

import { Button, Grid, Link } from '@mui/material';
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

var usernameText = "";
var passwordText = "";

function setUsernameText(str) {
    usernameText = str;
}

function setPasswordText(str) {
    passwordText = str;
}

export default function LoginPage() {
    const usernameRef = useRef(null);
    const passwordRef = useRef(null);

    const [username, setUsername] = useState({ value: "", error: false, hint: "" });
    const [password, setPassword] = useState({ value: "", error: false, hint: "" });

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
    }

    // action button
    const onClickReset = (event) => {
        setUsernameText("");
        setPasswordText("");
        setUsername({ ...username, value: "", error: false });
        setPassword({ ...password, value: "", error: false });
        usernameRef.current.getElementsByTagName("input")[0].value = "";
        passwordRef.current.getElementsByTagName("input")[0].value = "";
    }

    const onClickSubmit = async (event) => {
        if (!isReady()) {
            return;
        }

        setLoading(true);

        var dto = await stall(api.post("auth/login", {
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
    }, [username, password]);

    const checkField = (field) => {
        return (!field.error) && (field.value.length > 0)
    };

    const isReady = () => {
        return (checkField(username) && checkField(password));
    }

    // loading effect
    const [loading, setLoading] = useState(false);

    return (
        <div>
            <Helmet>
                <title>Sign in</title>
            </Helmet>
            <div className="form-main">
                <BackNavBar></BackNavBar>
                <div className="wrapper">
                    <div className="dialog">
                        <div className="title">
                            <h1 className="font-hand">Sign in</h1>
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
                        </div>

                        <div className="action-wrapper">
                            <div className='reset'>
                                <Button fullWidth variant='contained' onClick={loading ? null : onClickReset}>Reset</Button>
                            </div>
                            <div className='submit'>
                                <Button fullWidth variant='contained' color='success' disabled={!ready} onClick={loading ? null : onClickSubmit}>
                                    {loading ? <span>&nbsp;<FontAwesomeIcon icon={faSpinner} spinPulse />&nbsp;</span> : "Sign in"}
                                </Button>
                            </div>
                        </div>

                        <Grid container width={340} mt={2} justifyContent="flex-end">
                            <Grid item>
                                <Link href="/register" variant="body2">
                                    Do not have an account? Sign up
                                </Link>
                            </Grid>
                        </Grid>
                    </div>
                </div>
            </div>
        </div>
    );
}