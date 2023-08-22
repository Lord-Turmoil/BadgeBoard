import './QuestionNoteThumbnail.css';
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
        <div className="Note QuestionNote Note__Thumbnail">
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
};