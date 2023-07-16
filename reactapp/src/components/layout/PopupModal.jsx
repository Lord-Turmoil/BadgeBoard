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
        onClose && onClose();
    }

    const onConfirmInner = () => {
        onConfirm && onConfirm();
        onClose && onClose();
    }

    return (
        <Dialog open={open} onClose={onClose} scroll={scroll}>
            <DialogTitle>{title}</DialogTitle>
            <DialogContent dividers>
                {children}
            </DialogContent>
            <DialogActions>
                <Button autoFocus onClick={onCancelInner}>Cancel</Button>
                <Button onClick={onConfirmInner}>OK</Button>
            </DialogActions>
        </Dialog>
    );
};
