import { TextField } from '@mui/material';
import { useEffect, useState } from 'react';

export default function SubtleInput({ sx, cls = '', defaultValue = '', enabled = true, onChange = null }) {
    const [text, setText] = useState(defaultValue);

    useEffect(() => {
        console.log(text);
    }, [enabled]);

    return (
        <div className={`Subtle_Input ${cls ? cls : ''}`} style={{ ...sx }}>
            <div className="cover" style={{ display: enabled ? 'none' : 'inline-block' }}>{defaultValue}</div>
            <TextField defaultValue={text} variant="standard" sx={enabled ? null : { display: 'none' }}></TextField>
        </div>
    );
}