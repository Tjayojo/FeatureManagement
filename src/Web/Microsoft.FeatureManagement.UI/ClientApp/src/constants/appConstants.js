export const appInfo = {
  name: 'Arc IO'
};

export const appErrors = {
  genericError: 'Something went wrong',
  invalidOperation: 'Invalid Operation'
};

export const notificationConfiguration = {
  autoHideDuration: 4000,
  position: {
    vertical: 'bottom',
    horizontal: 'right'
  }
};

export const notificationType = {
  info: 'info',
  success: 'success',
  warning: 'warning',
  error: 'error',
  primary: 'primary'
};

const getApiBaseUrl = () => `${window.location.origin}/api`;

export const apiPaths = {
  featureUrl: `${getApiBaseUrl()}/Feature`,
  audienceUrl: `${getApiBaseUrl()}/Audience`,
  browserRestriction: `${getApiBaseUrl()}/BrowserRestriction`,
  groupRollout: `${getApiBaseUrl()}/GroupRollout`,
  timeWindow: `${getApiBaseUrl()}/TimeWindow`,
  user: `${getApiBaseUrl()}/User`,
  test: `${getApiBaseUrl()}/Test`
};