// default will show at top right
// parent div must be relative
import './CornerBadge.css';

export default function CornerBadge({
    size,
    className = null,
    sx = null,
    left = false,
    right = false,
    top = false,
    bottom = false,
    children
}) {
    const getPositionClass = () => {
        // left right real-right
        //   1     1      1
        //   1     0      0
        //   0     1      1
        //   0     0      1
        var isRight = !left || right;
        var isTop = !bottom || top;
        return (isRight ? 'right' : 'left') + ' ' + (isTop ? 'top' : 'bottom');
    }

    const getClassName = () => {
        return (className ? (className + ' ') : '') + getPositionClass();
    }

    return (
        <div className={`CornerBadge ${getClassName()}`} style={{ ...sx, width: size, height: size }}>
            {children}
        </div>
    );
};