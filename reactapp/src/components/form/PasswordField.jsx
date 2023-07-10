import React, { useState } from 'react';
import IconButton from '@mui/material/IconButton';
import InputLabel from '@mui/material/InputLabel';
import InputAdornment from '@mui/material/InputAdornment';
import FormControl from '@mui/material/FormControl';
import Visibility from '@mui/icons-material/Visibility';
import VisibilityOff from '@mui/icons-material/VisibilityOff';
import KeyRoundedIcon from '@mui/icons-material/KeyRounded';

import { Input } from '@mui/material';

export default function PasswordField(param) {
    const [props, setProps] = useState(param.sx ? param.sx : {});

    const [showPassword, setShowPassword] = useState(false);

    const handleClickShowPassword = () => setShowPassword((show) => !show);

    const handleMouseDownPassword = (event) => {
        event.preventDefault();
    };

    return (
        <div className={props.class} style={{ display: "flex", alignItems: "flex-end" }}>
            {props.icon ? props.icon : <KeyRoundedIcon fontSize='large'></KeyRoundedIcon>}
            <FormControl style={{ marginLeft: '10px' }} error={props.error} fullWidth variant="outlined">
                <InputLabel htmlFor="outlined-adornment-password">{props.label ? props.label : "Password"}</InputLabel>
                <Input
                    id={props.id}
                    type={showPassword ? 'text' : 'password'}
                    endAdornment={
                        <InputAdornment position="end">
                            <IconButton
                                aria-label="Toggle password visibility"
                                onClick={handleClickShowPassword}
                                onMouseDown={handleMouseDownPassword}
                                edge="end">
                                {showPassword ? <VisibilityOff /> : <Visibility />}
                            </IconButton>
                        </InputAdornment>
                    }
                    label={props.label ? props.label : "Password"}
                />
            </FormControl>
        </div>
    );
}