import './NoteModalTitle.css';

export default function NoteModalTitle({
    badge = null
}) {
    const renderBadgeTitle = (b) => {
        var title = "";
        if (b) {
            switch (b.type) {
                case 1:
                    title = "Question Badge";
                    break;
                case 2:
                    title = "Memory Badge";
                    break;
                default:
                    title = "Unknown Badge";
                    break;
            }
        } else {
            title = "???"
        }
        return <h3><i>{title}</i></h3>
    }

    return (
        <div className="NoteModalNav__title">
            {renderBadgeTitle(badge)}
        </div>
    );
}