import { TextField } from '@mui/material';
import { useEffect, useRef, useState } from 'react';

export default function SubtleInput({ sx, cls = '', defaultValue = '', enabled = true, onChange = null }) {
    let inputRef = useRef();
    const [text, setText] = useState(defaultValue);

    useEffect(() => {
        if (enabled) {
            inputRef.current.getElementsByTagName("input")[0].value = defaultValue;
        }
    }, [enabled]);
    return (
        <div className={`Subtle_Input ${cls ? cls : ''}`} style={{ ...sx }}>
            <div className="cover" style={{ display: enabled ? 'none' : 'inline-block' }}>{defaultValue}</div>
            <TextField ref={inputRef} variant="standard" sx={enabled ? null : { display: 'none' }}></TextField>
        </div>
    );
}