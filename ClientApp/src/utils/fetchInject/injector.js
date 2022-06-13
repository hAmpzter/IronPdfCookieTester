export const head = (namespace, win, doc, type, resource, resolve) => {
  const createEl = () => {
    const insertedEl = doc.getElementById(namespace)
    let el = insertedEl ? insertedEl : doc.createElement(type)

    if (!insertedEl) {
      el.setAttribute('id', namespace)
      el.appendChild(doc.createTextNode(resource.text))
    } else if (type === 'script') {
      el.parentNode.removeChild(el)
      el = null
      return createEl()
    }

    el.onload = resolve(resource)

    return el
  }

  const el = createEl()
  const tag = doc.getElementsByTagName(type)[0]

  tag ? tag.parentNode.insertBefore(el, tag) : doc.head.appendChild(el)
}
