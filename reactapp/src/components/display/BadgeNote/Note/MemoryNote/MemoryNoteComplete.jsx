export default function MemoryNoteComplete({
    badge = null
}) {
    const sender = badge ? badge.srcUser : null;
    const memory = badge ? badge.payload.memory : null;

    return (
        <div className="Note MemoryNote Note__Complete">
            <div className="MemoryNote__memory Note__boxWithSeparator">
                {sender ? <Avatar src={AvatarUtil.getUrlFromUser(sender)} /> : null}
                <h3>{memory}</h3>
            </div>
        </div>
    );
};