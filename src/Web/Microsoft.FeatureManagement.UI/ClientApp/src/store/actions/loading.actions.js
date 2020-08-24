const loadingActions = {
  START: 'LOADING_START',
  END: 'LOADING_END'
};

const loadingActionCreator = {
  increment: () => ({ type: loadingActions.START }),
  decrement: () => ({ type: loadingActions.END })
};

export { loadingActions, loadingActionCreator };