import React, { useEffect, useState } from 'react';
import IconButton from '@mui/material/IconButton';
import InputLabel from '@mui/material/InputLabel';
import InputAdornment from '@mui/material/InputAdornment';
import FormControl from '@mui/material/FormControl';
import Visibility from '@mui/icons-material/Visibility';
import VisibilityOff from '@mui/icons-material/VisibilityOff';
import KeyRoundedIcon from '@mui/icons-material/KeyRounded';
import { FormHelperText, Input } from '@mui/material';

// if 'error' wrapped in 'sx', its change won't be detected unless 'sx'
// changes wholely.
export default function PasswordField({ sx, error, disabled, hint, onChange }) {
    const [props, setProps] = useState(sx ? sx : {});

    const [showPassword, setShowPassword] = useState(false);

    const handleClickShowPassword = () => setShowPassword((show) => !show);

    const handleMouseDownPassword = (event) => {
        event.preventDefault();
    };

    return (
        <div className={props.class} style={{ display: 'flex', alignItems: 'flex-end' }}>
            {props.icon ? props.icon : <KeyRoundedIcon fontSize='large'></KeyRoundedIcon>}
            <FormControl style={{ marginLeft: '10px' }} error={error} fullWidth variant="outlined">
                <InputLabel htmlFor="outlined-adornment-password">{props.label ? props.label : 'Password'}</InputLabel>
                <Input
                    id={props.id}
                    ref={props.ref}
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
                    label={props.label ? props.label : 'Password'}
                    onChange={onChange}
                    disabled={disabled}
                />
                <FormHelperText style={{
                    position: 'absolute',
                    top: '95%',
                    opacity: error ? '100%' : '0%',
                    transition: '0.3s'
                }}>{hint}</FormHelperText>
            </FormControl>
        </div>
    );
}