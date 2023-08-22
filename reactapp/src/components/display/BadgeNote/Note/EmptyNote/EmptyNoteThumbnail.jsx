import './EmptyNoteThumbnail.css';
import '../NoteDecoration.css';

// sender & receiver should be (full user)
export default function EmptyNote({
    text = "",
}) {
    return (
        <div className="Note EmptyNote Note__Thumbnail">
            <div className="EmptyNote__text Note__boxWithSeparator">
                <h3>{text}</h3>
            </div>
        </div>
    );
};