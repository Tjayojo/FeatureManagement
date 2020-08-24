import { loadingActions } from '../actions';

const initialState = { inProgress: 0 };

export const loadingReducer = (state, action) => {
  state = state || initialState;

  if (action.type === loadingActions.START) {
    return { ...state, inProgress: state.inProgress + 1 };
  }

  if (action.type === loadingActions.END) {
    if (state.inProgress === 0) {
      return state;
    }
    return { ...state, inProgress: state.inProgress - 1 };
  }

  return state;
};
