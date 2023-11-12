var _paq = window._paq = window._paq || [];
_paq.push(['trackPageView']);
_paq.push(['enableLinkTracking']);

export function init(apiUrl, siteId) {
  console.log("registering", apiUrl, siteId);
  var u = apiUrl;
  u += u.endsWith("/") ? "" : "/"
  window._paq.push(['setTrackerUrl', u + 'matomo.php']);
  window._paq.push(['setSiteId', siteId]);
  var d = document, g = d.createElement('script'), s = d.getElementsByTagName('script')[0];
  g.type = 'text/javascript'; g.async = true; g.defer = true; g.src = u + 'matomo.js'; s.parentNode.insertBefore(g, s);
}
export function triggerEvent(url, userId) {
  console.log("Tracking", url);
  window._paq.push(['setCustomUrl', url]);
  if (userId) {
    window._paq.push(['setUserId', userId]);
  }
  window._paq.push(['trackPageView']);
}
