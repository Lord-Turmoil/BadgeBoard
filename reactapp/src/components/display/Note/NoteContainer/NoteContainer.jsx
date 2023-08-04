import { useRef } from 'react';
import './NoteContainer.css';

export default function StickyNote({
    rotate = 0,
    random = true,
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

    return (
        <div className="NoteContainer" style={{ transform: rotation.current }}>
            {children}
        </div>
    );
};