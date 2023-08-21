import { useCallback, useEffect, useRef, useState } from "react";
import BadgeContainerMobile from "~/components/layout/BadgeContainer/BadgeContainerMobile";
import NoteContainer from '~/components/display/Note/NoteContainer/NoteContainer';
import QuestionNote from '~/components/display/Note/QuestionNote/QuestionNote';
import { CircularProgress } from "@mui/material";

import './BadgeBoardMobile.css';
import useInterval from "~/services/hook/useInterval";
import MemoryNote from "~/components/display/Note/MemoryNote/MemoryNote";
import EmptyNote from "~/components/display/Note/EmptyNote/EmptyNote";

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
            return (
                <QuestionNote
                    question={badge.payload.question}
                    answer={badge.payload.answer}
                    sender={badge.srcUser}
                    receiver={badge.dstUser} />
            );
        } else if (badge.type == 2) {
            return (
                <MemoryNote memory={badge.payload.memory} sender={badge.srcUser} />
            );
        } else {
            return <EmptyNote text="Sorry, but this badge was corrupted." />
        }
    }
    const renderBadge = (badge, index) => {
        return (
            <NoteContainer rotate={6} variant={badge.style} key={badge.id}>
                {renderPayload(badge)}
            </NoteContainer>
        );
    }
    const renderEmptyBadge = () => {
        return (
            <NoteContainer rotate={6}>
                <EmptyNote text="There is no badge here, click '+' and place one now!" />
            </NoteContainer>
        );
    }

    const isLoading = Boolean(!badges);
    const isEmpty = Boolean(!badges || badges.count == 0);

    return (
        <BadgeContainerMobile className={`BadgeBoard BadgeBoardMobile${badges ? "" : " loading"}`}>
            {isLoading ?
                <div className="BadgeBoard__loading">
                    <CircularProgress size='60%' color="secondary" />
                    <h3>{loadingText}</h3>
                </div>
                :
                <div className="BadgeBoard__board">
                    {
                        badges.badges.map((badge, index) => {
                            return renderBadge(badge, index);
                        })
                    }
                    {isEmpty && renderEmptyBadge()}
                </div>
            }
        </BadgeContainerMobile>
    );
};