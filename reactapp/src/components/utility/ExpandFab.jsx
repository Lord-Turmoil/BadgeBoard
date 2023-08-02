import { ExpandLessRounded, ExpandMoreRounded } from '@mui/icons-material';
import { Fab } from '@mui/material';
import { forwardRef, useState } from 'react';

export function ExpandFab({
    disabled = false,
    open = false,
    setOpen = null,
}, ref) {
    const handleClick = () => {
        setOpen(!open);
    }
    return (
        <Fab ref={ref} sx={{ flexShrink: 0 }} disabled={disabled} color="primary" onClick={handleClick}>
            <ExpandMoreRounded fontSize="large" sx={{
                transition: 'transform 0.3s',
                transform: open ? 'rotate(0deg)' : 'rotate(180deg)'
            }} />
        </Fab>
    );
}

const forwardedRef = forwardRef(ExpandFab);

// Exporting the wrapped component
export default forwardedRef;