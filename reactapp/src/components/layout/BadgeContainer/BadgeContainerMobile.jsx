import './BadgeContainerMobile.css'
export default function BadgeContainerMobile({
    children
}) {
    return (
        <div className="BadgeContainer BadgeContainerMobile">
            {children}
        </div>
    );
};