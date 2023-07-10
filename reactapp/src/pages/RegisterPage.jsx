import React, { useEffect, useRef, useState } from 'react';
import '../assets/css/pages/register.css'
import { Helmet } from 'react-helmet';
import PasswordField from '../components/form/PasswordField';
import PlainTextField from '../components/form/PlainTextField'
import BackNavBar from '../components/form/BackNavBar';
import AccountCircleRoundedIcon from '@mui/icons-material/AccountCircleRounded';
import _debounce from 'debounce';

// const PASSWORD_REGEX = new RegExp(/^(?=.*\d)(?=(.*\W){1})(?=.*[a-zA-Z])(?!.*\s).{6,16}$/);
const PASSWORD_REGEX = new RegExp(/^[a-z0-9_-]{6,16}$/i);
const USERNAME_REGEX = new RegExp(/^[ a-z0-9_-]{3,20}$/);

export default function RegisterPage() {
    const usernameRef = useRef(null);
    const [password, setPassword] = useState({ value: "", error: false });
    const [confirm, setConfirm] = useState({ value: "", error: false, hint: "Passwords inconsistent" });
    const [username, setUsername] = useState({ value: "", error: false });

    const onUsernameChange = (event) => {
        var newUsername = event.target.value;
        if ((newUsername.length > 0) && !USERNAME_REGEX.test(newUsername)) {
            setUsername({ ...username, value: newUsername, error: true, hint: "3 ~ 20 characters (a-zA-Z0-9_-)" });
        } else {
            setUsername({ ...username, value: newUsername, error: false });
        }
    }

    const onPasswordChange = (event) => {
        // it is async, so won't be updated right away
        var newPassword = event.target.value;
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
    }, [password]);

    useEffect(() => {
        checkConfirm();
    }, [confirm]);

    const checkConfirm = () => {
        if (confirm.value !== password.value) {
            if (!confirm.error) {
                setConfirm({ ...confirm, error: true });
            }
        } else if (confirm.error) {
            setConfirm({ ...confirm, error: false });
        }
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
                                    onChange={_debounce(onUsernameChange, 500)}
                                    sx={{
                                        id: 'username', ref: usernameRef, label: 'Username',
                                        icon: <AccountCircleRoundedIcon fontSize='large' />
                                    }} />
                            </div>
                            <div className="input-item">
                                <PasswordField
                                    error={password.error}
                                    hint={password.hint}
                                    onChange={_debounce(onPasswordChange, 500)}
                                    sx={{
                                        id: 'password'
                                    }} />
                            </div>
                            <div className="input-item">
                                <PasswordField
                                    error={confirm.error}
                                    hint={confirm.hint}
                                    onChange={_debounce(onConfirmChange, 500)}
                                    sx={{ id: 'confirm', label: 'Confirm Password' }} />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}