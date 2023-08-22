import { Box, Modal } from "@mui/material";
import './NoteModal.css'
import '~/assets/css/note-style.css';
import 'animate.css';

const style = {
    position: 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    width: '90%'
};

export default function NoteModal({
    open = false,
    onClose = null,
    badge = null,
    categories = null, // move
    user = null,       // whether editable
}) {
    return (
        <Modal open={open} onClose={onClose} className="NoteModal">
            <div className={`NoteModal__note StyledNote ${badge && badge.style}`} style={style}>
                <h1>HELLO</h1>
            </div>
        </Modal>
    );
};
