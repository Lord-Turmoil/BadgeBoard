import { ExpandLessRounded, ExpandMoreRounded } from '@mui/icons-material';
import { Fab } from '@mui/material';
import { useState } from 'react';

export default function ExpandFab({ disabled = false, onClick = null }) {
    const [expandOn, setExpandOn] = useState(false);

    function toggleOn() {
        setExpandOn(!expandOn);
        onClick && onClick();
    }

    return (
        <Fab disabled={disabled} color="primary" onClick={toggleOn}>
            <ExpandMoreRounded fontSize="large" sx={{
                transition: 'transform 0.3s',
                transform: expandOn ? 'rotate(0deg)' : 'rotate(180deg)'
            }} />
        </Fab>
    );
}
