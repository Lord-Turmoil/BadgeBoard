import { Box, Modal } from "@mui/material";
import './NoteModal.css'
import '~/assets/css/note-style.css';
import 'animate.css';
import NoteModalNav from "./NoteModalNav/NoteModalNav";

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
    onBadgeChange = null,
    categories = null, // move
    user = null,       // whether editable
}) {
    const isOwner = Boolean((badge && user) && badge.receiver == user.account.id);

    return (
        <Modal open={open} onClose={onClose} className="NoteModal">
            <div className={`NoteModal__note StyledNote ${badge && badge.style}`} style={style}>
                <NoteModalNav badge={badge} onBadgeChange={onBadgeChange} isOwner={isOwner} />
                
            </div>
        </Modal>
    );
};
