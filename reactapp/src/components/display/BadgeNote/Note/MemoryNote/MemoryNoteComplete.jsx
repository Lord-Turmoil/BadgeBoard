export default function MemoryNoteComplete({
}) {
    return (
        <div className="Note MemoryNote Note__Complete">
            <div className="MemoryNote__memory Note__boxWithSeparator">
                {sender && <Avatar src={AvatarUtil.getUrlFromUser(sender)} />}
                <h3>{memory}</h3>
            </div>
        </div>
    );
};