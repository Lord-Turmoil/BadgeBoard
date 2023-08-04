import './BadgeContainerMobile.css'
export default function BadgeContainerMobile({
    className = null,
    children
}) {
    return (
        <div className={`BadgeContainer BadgeContainerMobile ${className}`}>
            {children}
        </div>
    );
};