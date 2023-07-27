import { ExpandLessRounded, ExpandMoreRounded } from '@mui/icons-material';
import { Fab } from '@mui/material';
import { useState } from 'react';

export default function ExpandFab({ disabled = false, open = false, onClick = null }) {
    return (
        <Fab disabled={disabled} color="primary" onClick={onClick}>
            <ExpandMoreRounded fontSize="large" sx={{
                transition: 'transform 0.3s',
                transform: open ? 'rotate(0deg)' : 'rotate(180deg)'
            }}/>
        </Fab>
    );
}