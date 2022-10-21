var _paq = _paq || [];
_paq.push(['trackPageView']);
_paq.push(['enableLinkTracking']);

export function init(apiUrl, siteId) {
  console.log("registering", apiUrl, siteId);
  var u = apiUrl;
  u += u.endsWith("/") ? "" : "/"
  _paq.push(['setTrackerUrl', u + 'matomo.php']);
  _paq.push(['setSiteId', siteId]);
  var d = document, g = d.createElement('script'), s = d.getElementsByTagName('script')[0];
  g.type = 'text/javascript'; g.async = true; g.defer = true; g.src = u + 'matomo.js'; s.parentNode.insertBefore(g, s);
}
export function triggerEvent(url, userId) {
  console.log("Tracking", url);
  _paq.push(['setCustomUrl', url]);
  _paq.push(['trackPageView']);
  if (userId) {
    _paq.push(['setUserId', userId]);
  }
}
