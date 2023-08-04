import './QuestionNote.css';
import '../NoteDecoration.css';

export default function QuestionNote({
    question = "",
    answer = null
}) {
    return (
        <div className="Note QuestionNote">
            <div className="QuestionNote__question Note__boxWithSeparator">
                <h3>{question}</h3>
            </div>
            <div className="QuestionNote__answer">
                <p className={answer ? null : 'empty'}>{answer ?? "Not answered yet..."}</p>
            </div>
        </div>
    );
};