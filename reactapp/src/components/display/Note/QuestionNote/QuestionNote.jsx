import './QuestionNote.css';
import '../NoteDecoration.css';
import { Avatar } from '@mui/material';
import AvatarUtil from '~/services/user/AvatarUtil';

// sender & receiver should be (full user)
export default function QuestionNote({
    question = "",
    answer = null,
    sender = null,
    receiver = null
}) {
    return (
        <div className="Note QuestionNote">
            <div className="QuestionNote__question Note__boxWithSeparator">
                {sender && <Avatar src={AvatarUtil.getUrlFromUser(sender)} />}
                <h3>{question}</h3>
            </div>
            <div className="QuestionNote__answer">
                <p className={answer ? null : 'empty'}>{answer ?? "Not answered yet..."}</p>
                <Avatar className='QuestionNote__avatar' src={AvatarUtil.getUrlFromUser(receiver)} />
            </div>
        </div>
    );
};