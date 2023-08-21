import './EmptyNote.css';
import '../NoteDecoration.css';

// sender & receiver should be (full user)
export default function EmptyNote({
    text = "",
}) {
    return (
        <div className="Note EmptyNote">
            <div className="EmptyNote__text Note__boxWithSeparator">
                <h3>{text}</h3>
            </div>
        </div>
    );
};