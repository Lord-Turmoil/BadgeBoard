// default will show at top right
// parent div must be relative
import './CornerBadge.css';

export default function CornerBadge({
    size,
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

    return (
        <div className={`CornerBadge ${getPositionClass()}`} style={sx}>
            {children}
        </div>
    );
};