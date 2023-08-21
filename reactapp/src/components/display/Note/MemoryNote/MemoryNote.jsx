import './MemoryNote.css';
import '../NoteDecoration.css';
import { Avatar } from '@mui/material';
import AvatarUtil from '~/services/user/AvatarUtil';

// sender & receiver should be (full user)
export default function MemoryNote({
    memory = "",
    sender = null
}) {
    return (
        <div className="Note MemoryNote">
            <div className="MemoryNote__memory Note__boxWithSeparator">
                {sender && <Avatar src={AvatarUtil.getUrlFromUser(sender)} />}
                <h3>{memory}</h3>
            </div>
        </div>
    );
};