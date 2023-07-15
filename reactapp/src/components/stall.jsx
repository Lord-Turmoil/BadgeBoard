export function delay(ms) {
    return new Promise((resolve) => {
        setTimeout(() => { resolve() }, ms);
    });
}

// make task complete at least 'ms' ms later
export const EXTRA_STALL = 2000;
export const LONG_STALL = 1000;
export const NORMAL_STALL = 500;
export const SHORT_STALL = 300;
export const ACTIVE_STALL = NORMAL_STALL;
export default async function stall(task, ms = ACTIVE_STALL) {
    var [ret, _] = await Promise.all([task, delay(ms)]);
    return ret;
}