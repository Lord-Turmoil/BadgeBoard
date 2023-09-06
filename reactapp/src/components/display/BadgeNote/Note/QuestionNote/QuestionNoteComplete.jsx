import './QuestionNote.css';
import '../NoteDecoration.css';
import { Avatar } from '@mui/material';
import AvatarUtil from '~/services/user/AvatarUtil';

export default function QuestionNoteComplete({
    badge = null
}) {
    const sender = badge ? badge.srcUser : null;
    const receiver = badge ? badge.dstUser : null;
    const question = badge ? badge.payload.question : null;
    const answer = badge ? badge.payload.answer : null;

    return (
        <div className="Note QuestionNote Note__Complete">
            <div className="QuestionNote__question Note__boxWithSeparator" style={{ marginBottom: "20px" }}>
                {sender && <Avatar sx={{ mr: '5px' }} src={AvatarUtil.getUrlFromUser(sender)} />}
                <h3>{question}</h3>
            </div>
            <div className="QuestionNote__answer">
                <p className={answer ? null : 'empty'}>{answer ?? "Not answered yet..."}</p>
                {answer && <Avatar className='QuestionNote__avatar' src={AvatarUtil.getUrlFromUser(receiver)} />}
            </div>
        </div>
    );
}