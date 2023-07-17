import { Button, Dialog, DialogActions, DialogContent, DialogTitle } from "@mui/material";

export default function PopupModal({
    sx = {},
    open = false,
    scroll = "body",
    onClose = null,
    onConfirm = null,
    onCancel = null,
    title = "Dialog",
    children
}) {
    const onCancelInner = () => {
        onCancel && onCancel();
    }

    const onConfirmInner = () => {
        onConfirm && onConfirm();
    }

    return (
        <Dialog open={open} onClose={onClose} scroll={scroll} fullWidth>
            <DialogTitle>{title}</DialogTitle>
            <DialogContent dividers>
                {children}
            </DialogContent>
            <DialogActions>
                <Button variant="contained" color="error" autoFocus onClick={onCancelInner}>Cancel</Button>
                <Button variant="contained" color="success" onClick={onConfirmInner}>OK</Button>
            </DialogActions>
        </Dialog>
    );
};
