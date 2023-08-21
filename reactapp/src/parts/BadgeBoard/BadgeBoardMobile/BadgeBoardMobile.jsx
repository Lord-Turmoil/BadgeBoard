import { useCallback, useEffect, useRef, useState } from "react";
import BadgeContainerMobile from "~/components/layout/BadgeContainer/BadgeContainerMobile";
import NoteContainer from '~/components/display/Note/NoteContainer/NoteContainer';
import QuestionNote from '~/components/display/Note/QuestionNote/QuestionNote';
import { CircularProgress } from "@mui/material";

import './BadgeBoardMobile.css';
import useInterval from "~/services/hook/useInterval";

export default function BadgeBoardMobile({
    badges = null
}) {
    const [loadingText, setLoadingText] = useState();
    const [loadingCount, setLoadingCount] = useState(0);

    useInterval(() => {
        var text = "Loading";
        for (var i = 0; i < loadingCount; i++) {
            text += ".";
        }
        setLoadingText(text);
        setLoadingCount(loadingCount >= 3 ? 0 : loadingCount + 1);
    }, 500);

    const renderPayload = (badge) => {
        if (badge.type == 1) {
            return (<QuestionNote question={badge.payload.question} answer={badge.payload.answer} />)
        } else if (badge.type == 2) {
            return <div></div>;
        } else {
            return <div></div>
        }
    }
    const renderBadge = (badge) => {
        return (
            <NoteContainer rotate={8} variant={badge.style} key={badge.id}>
                {renderPayload(badge)}
            </NoteContainer>
        );
    }

    return (
        <BadgeContainerMobile className={`BadgeBoard BadgeBoardMobile${badges ? "" : " loading"}`}>
            {badges ?
                <div className="BadgeBoard__board">
                    {badges.badges.map((badge, index) => {
                        return renderBadge(badge);
                    })}
                </div>
                :
                <div className="BadgeBoard__loading">
                    <CircularProgress size='60%' color="secondary" />
                    <h3>{loadingText}</h3>
                </div>
            }
        </BadgeContainerMobile>
    );
};