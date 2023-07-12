import React, { useState } from 'react';
import InputLabel from '@mui/material/InputLabel';
import FormControl from '@mui/material/FormControl';
import TextFieldsRoundedIcon from '@mui/icons-material/TextFieldsRounded';

import { FormHelperText, Input } from '@mui/material';

export default function PlainTextField({ sx, error, hint, disabled, onChange }) {
    const [props, setProps] = useState(sx ? sx : {});

    return (
        <div className={props.class} style={{ display: "flex", alignItems: "flex-end" }}>
            {props.icon ? props.icon : <TextFieldsRoundedIcon fontSize='large'></TextFieldsRoundedIcon>}
            <FormControl style={{ marginLeft: '10px' }} error={error} fullWidth variant="outlined">
                <InputLabel htmlFor="outlined-adornment-password">{props.label ? props.label : "Text"}</InputLabel>
                <Input
                    id={props.id}
                    ref={props.ref}
                    type='text'
                    label={props.label ? props.label : "Text"}
                    onChange={onChange}
                    disabled={disabled}
                />
                <FormHelperText style={{
                    position: 'absolute',
                    top: '95%',
                    opacity: error ? "100%" : "0%",
                    transition: "0.3s"
                }}>{hint}</FormHelperText>
            </FormControl>
        </div>
    );
}