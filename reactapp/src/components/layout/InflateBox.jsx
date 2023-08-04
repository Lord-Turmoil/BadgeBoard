import { useEffect, useState } from 'react';

// Anything between opening and closing tags goes into children prop.
export default function InflateBox({ children, overflow=false, sx={} }) {
    const [height, setHeight] = useState(overflow ? 'auto' : '100%');
    const [minHeight, setMinHeight] = useState('100%');

    function onResize() {
        const newHeight = window.innerHeight + 'px';
        setMinHeight(newHeight);
        setHeight(overflow ? 'auto' : newHeight);
    }

    useEffect(() => {
            onResize();
            window.removeEventListener('resize', onResize);
            window.addEventListener('resize', onResize);
        },
        []);

    return (
        <div className="Inflate_Box clearfix" style={{ ...sx, height: height, minHeight: minHeight }}>
            {children}
        </div>
    );
}