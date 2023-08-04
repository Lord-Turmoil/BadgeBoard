import CornerBadge from "~/components/display/Badge/CornerBadge/CornerBadge"

import './TitleBadge.css';

export default function TitleBadge({ title }) {
    const getTitleType = (t) => {
        if (t == null) {
            return 0;
        }
        switch (t.toLowerCase()) {
            case 'padawan': return 0;
            case 'master': return 1;
            case 'lord': return 2;
            default: return 0;
        }
    }

    const getStyleClass = (t) => {
        switch (t) {
            case 0: return 'padawan';
            case 1: return 'master';
            case 2: return 'lord';
            default: return 'padawan';
        }
    }

    const renderTitle = (t) => {
        switch (t) {
            case 0: return (<span>Padawan</span>);
            case 1: return (<span>Master</span>);
            case 2: return (<span>Lord</span>);
            default: return (<span>Pirate</span>); //never reach
        }
    }

    const type = getTitleType(title);
    return (
        <CornerBadge className='TitleBadge' right top>
            <div className={`TitleBadge__badge ${getStyleClass(type)}`}>
                {renderTitle(type)}
            </div>
        </CornerBadge>
    );
};