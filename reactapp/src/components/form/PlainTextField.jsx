import React, { useState } from 'react';
import InputLabel from '@mui/material/InputLabel';
import FormControl from '@mui/material/FormControl';
import TextFieldsRoundedIcon from '@mui/icons-material/TextFieldsRounded';

import { Input } from '@mui/material';

export default function PlainTextField(param) {
    const [props, setProps] = useState(param.sx ? param.sx : {});

    return (
        <div className={props.class} style={{ display: "flex", alignItems: "flex-end" }}>
            {props.icon ? props.icon : <TextFieldsRoundedIcon fontSize='large'></TextFieldsRoundedIcon>}
            <FormControl style={{ marginLeft: '10px' }} error={props.error} fullWidth variant="outlined">
                <InputLabel htmlFor="outlined-adornment-password">{props.label ? props.label : "Text"}</InputLabel>
                <Input
                    id={props.id}
                    type='text'
                    label={props.label ? props.label : "Text"}
                />
            </FormControl>
        </div>
    );
}