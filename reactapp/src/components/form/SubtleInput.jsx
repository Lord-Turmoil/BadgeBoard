import { TextField } from '@mui/material';
import { useEffect, useRef, useState } from 'react';

export default function SubtleInput({
    sx = null,
    cls = '',
    placeholder = '',
    error = false,
    helperText = '',
    multiline = false,
    defaultValue = '',
    enabled = true,
    onChange = null,
    variant = 'standard',
    label = null
}) {
    const inputRef = useRef();
    const [text, setText] = useState(defaultValue);

    useEffect(() => {
            if (enabled) {
                if (multiline) {
                    inputRef.current.getElementsByTagName('textarea')[0].value = defaultValue;
                } else {
                    inputRef.current.getElementsByTagName('input')[0].value = defaultValue;
                }
            }
        },
        [enabled]);

    return (
        <div className={`Subtle_Input ${cls ? cls : ''}`} style={{ ...sx }}>
            <div className="cover" style={{ display: enabled ? 'none' : 'inline-block' }}>{defaultValue}</div>
            <TextField
                fullWidth
                error={error}
                placeholder={placeholder}
                helperText={error ? helperText : ''}
                multiline={multiline}
                maxRows={5}
                ref={inputRef}
                variant={variant}
                label={label}
                sx={enabled ? null : { display: 'none' }}
                onChange={onChange}/>
        </div>
    );
}