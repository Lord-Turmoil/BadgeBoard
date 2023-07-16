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
    return (
        <Dialog open={open} onClose={onClose} scroll={scroll}>
            <DialogTitle>{title}</DialogTitle>
            <DialogContent dividers>
                {children}
            </DialogContent>
            <DialogActions>
                <Button autoFocus onClick={onCancel ?? onClose}>Cancel</Button>
                <Button onClick={onConfirm ?? onClose}>OK</Button>
            </DialogActions>
        </Dialog>
    );
};
