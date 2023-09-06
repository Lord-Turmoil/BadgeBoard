import { Box, Modal } from "@mui/material";
import './NoteModal.css'
import '~/assets/css/note-style.css';
import 'animate.css';
import NoteModalNav from "./NoteModalNav/NoteModalNav";
import MemoryNoteComplete from "../Note/MemoryNote/MemoryNoteComplete";
import QuestionNoteComplete from "../Note/QuestionNote/QuestionNoteComplete";

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

    const renderBadge = (b) => {
        if (!b) {
            return null;
        }
        if (b.type == 1) {
            return (<QuestionNoteComplete badge={b} />);
        } else if (b.type == 2) {
            return (<MemoryNoteComplete badge={b} />);
        } else {
            console.log("Invalid type");
            return null;
        }
    }

    return (
        <Modal open={open} onClose={onClose} className="NoteModal">
            <div className={`NoteModal__note StyledNote ${badge && badge.style}`} style={style}>
                <NoteModalNav badge={badge} onBadgeChange={onBadgeChange} isOwner={isOwner} onClose={onClose} />
                {renderBadge(badge)}
            </div>
        </Modal>
    );
};
