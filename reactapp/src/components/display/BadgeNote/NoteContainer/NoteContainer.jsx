import { useRef } from 'react';
import './NoteContainer.css';
import 'animate.css';
import '~/assets/css/note-style.css';

export default function NoteContainer({
    rotate = 0,
    random = true,
    variant = null,
    onClick = null,
    className = null,
    children
}) {
    // if random is true, then will rotate at random degree, taking 'rotate' as
    // max rotating angle. Else, will rotate at fixed degree as rotate.
    const getDegree = () => {
        if (rotate === null) {
            rotate = 0;
        }
        if (random) {
            return (Math.random() - 0.5) * rotate;
        }
        return rotate;
    }

    const getRotation = () => {
        return 'rotate(' + getDegree() + 'deg)';
    }

    const rotation = useRef(getRotation());

    const animateStyle = "animate__animated animate__zoomIn animate__faster";

    return (
        <div className={`NoteContainer StyledNote ${animateStyle} ${className ? className : ""} ${variant ? variant : ""}`}
            style={{ transform: rotation.current }}
            onClick={onClick}>
            {children}
        </div>
    );
};