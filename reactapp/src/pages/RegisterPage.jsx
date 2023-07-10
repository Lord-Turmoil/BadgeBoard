import React from 'react';
import '../assets/css/pages/register.css'
import { Helmet } from 'react-helmet';
import PasswordField from '../components/form/PasswordField';
import PlainTextField from '../components/form/PlainTextField'
import BackNavBar from '../components/form/BackNavBar';
import AccountCircleRoundedIcon from '@mui/icons-material/AccountCircleRounded';

export default function RegisterPage() {
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
                                <PlainTextField sx={{
                                    id: 'username', label: 'Username',
                                    icon: <AccountCircleRoundedIcon fontSize='large' />
                                }}></PlainTextField>
                            </div>
                            <div className="input-item">
                                <PasswordField sx={{ id: 'password' }}></PasswordField>
                            </div>
                            <div className="input-item">
                                <PasswordField sx={{ id: 'confirm', label: 'Confirm Password'}}></PasswordField>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}
