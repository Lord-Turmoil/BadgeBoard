import { useState } from 'react';
import SpeedDial from '@mui/material/SpeedDial';
import SpeedDialIcon from '@mui/material/SpeedDialIcon';
import SpeedDialAction from '@mui/material/SpeedDialAction';
import EditIcon from '@mui/icons-material/Edit';

import './BadgeActionDial.css';
import { getActions } from '~/services/action/ActionUtil';

export default function BadgeActionDial({
    user = null,
    visitor = null,
    category = null,
    actions = null
}) {
    const [open, setOpen] = useState(false);
    const handleOpen = () => setOpen(true);
    const handleClose = () => setOpen(false);

    const handleClick = (action) => {
        if (action && action.callback) {
            action.callback();
        }
        handleClose();
    };

    const hidden = Boolean(!actions);

    return (
        <SpeedDial
            hidden={hidden}
            ariaLabel="Action Dial"
            sx={{ position: 'fixed', bottom: 16, right: 16 }}
            icon={<SpeedDialIcon openIcon={<EditIcon />} />}
            onClose={handleClose}
            onOpen={handleOpen}
            open={open}
        >
            {actions && actions.map((action) => (
                <SpeedDialAction
                    key={action.name}
                    icon={action.icon}
                    tooltipOpen
                    tooltipTitle={action.name}
                    onClick={() => { handleClick(action) }}
                />
            ))}
        </SpeedDial>
    );
}