import { ClickAwayListener, Grow, MenuList, Paper, Popper } from '@mui/material';

export default function PopperMenu({
    open,
    onClose = null,
    anchorEl,
    placement = 'bottom-start',
    transformOrigin = 'left top',
    children
}) {
    const handleClickAway = () => {
        onClose && onClose();
    };

    const handleListKeyDown = (event) => {
        if (event.key === 'Tab') {
            event.preventDefault();
            onClose && onClose();
        } else if (event.key === 'Escape') {
            onClose && onClose();
        }
    };

    return (
        <Popper
            open={open}
            anchorEl={anchorEl}
            placement={placement}
            transition
            disablePortal>
            {({ TransitionProps, placement }) => (
                <Grow
                    {...TransitionProps}
                    style={{ transformOrigin: transformOrigin }}>
                    <Paper>
                        <ClickAwayListener onClickAway={handleClickAway}>
                            <MenuList autoFocusItem={open} onKeyDown={handleListKeyDown}>
                                { children }
                            </MenuList>
                        </ClickAwayListener>
                    </Paper>
                </Grow>
            )}
        </Popper>
    );
}