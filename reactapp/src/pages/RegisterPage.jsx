import React, { useEffect } from 'react';
import ArrowBackIosRoundedIcon from '@mui/icons-material/ArrowBackIosRounded';
import '../assets/css/pages/register.css'
import { Fab } from '@mui/material';
import { Helmet } from 'react-helmet';

export default function RegisterPage() {
    return (
        <div>
            <Helmet>
                <title>Sign up</title>
            </Helmet>
            <div className="register-main">
                <div className="wrapper">
                    <div className="title-wrapper">
                        <div className="back">
                            <Fab size='medium' color='primary'>
                                <ArrowBackIosRoundedIcon />
                            </Fab>
                        </div>
                        <div className="title">
                            <h1 className='font-hand'>Sign Up</h1>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}
