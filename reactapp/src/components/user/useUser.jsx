import { useEffect, useState } from "react";
import api from "../api";
import User from "./user";

export const useLocalUser = () => {
    const [data, setData] = useState(null);
    useEffect(() => {
        setData(User.get());
    }, []);

    return { data };
}

export const useUser = (uid) => {
    const [data, setData] = useState(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    useEffect(() => {
        let didCancel = false;

        setError(null);
        if (uid) {
            (async () => {
                try {
                    setLoading(true);
                    var dto = await api.get("user/user", { id: uid });
                    if (dto.meta.status != 0) {
                        throw new Error(dto.meta.message);
                    }
                    setData(dto.data);
                    console.log("🚀 ~ file: useUser.jsx:32 ~ dto.data:", dto.data)
                } catch (err) {
                    setError(err);
                } finally {
                    setLoading(false);
                }
            })();
        } else {
            setData(null);
            setLoading(false);
        }
        return () => {
            didCancel = true;
        }
    }, [uid]);

    return { data, loading, error };
}