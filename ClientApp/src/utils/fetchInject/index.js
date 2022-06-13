import { head as injectHead } from './injector'
//import { channels } from 'myb-common-js'

/**
 * Fetch Inject module.
 *
 * @module fetchInject
 * @license ISC
 * @param {String} namespace for script and style tags.
 * @param {(USVString[]|Request[])} inputs Resources you wish to fetch.
 * @param {Promise} [promise] A promise to await before attempting injection.
 * @throws {Promise<TypeError>} Rejects with error on invalid arguments.
 * @throws {Promise<Error>} Whatever `fetch` decides to throw.
 * @throws {SyntaxError} Via DOM upon attempting to parse unexpected tokens.
 * @returns {Promise<Object[]>} A promise which resolves to an `Array` of
 *     Objects containing `Response` `Body` properties used by the module.
 */
const fetchInject = function(inputs, promise) {
  if (!(inputs && Array.isArray(inputs)))
    return Promise.reject(new TypeError('`inputs` must be an array'))
  if (promise && !(promise instanceof Promise))
    return Promise.reject(new TypeError('`promise` must be a promise'))

  const resources = []
  const deferreds = promise ? [].concat(promise) : []
  const thenables = []

  inputs.forEach(input =>
    deferreds.push(
      window
        .fetch(input, {
          method: 'GET',
          credentials: 'same-origin',
        })
        .then(res => {
          return [res.clone().text(), res.blob()]
        })
        .then(promises => {
          return Promise.all(promises).then(resolved => {
            resources.push({ url: input, text: resolved[0], blob: resolved[1] })
          })
        }),
    ),
  )

  return Promise.all(deferreds).then(() => {
    resources.forEach(resource => {
      const namespace = resource.url.replace(new RegExp(/\//, 'g'), '-').split('?')[0]
      const blobTypeCss = resource.blob.type.includes('text/css')
      const blobTypeJson = resource.blob.type.includes('application/json')
      const blobTypeJs = resource.blob.type.includes('application/javascript')

      thenables.push({
        then: resolve => {
          if (blobTypeCss) {
            injectHead(namespace, window, document, 'style', resource, resolve)
          } else if (blobTypeJs) {
            injectHead(namespace, window, document, 'script', resource, resolve)
          } else if (blobTypeJson) {
            resolve({ ...resource, ...JSON.parse(resource.text) })
          } else {
            resolve(resource)
          }
        },
      })
    })

    return Promise.all(thenables)
  })
}

export default fetchInject
